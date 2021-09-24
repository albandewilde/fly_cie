package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"strings"
)

const backURL = "http://server"

// TODO listing flights
func main() {
	http.HandleFunc("/alive/", func(w http.ResponseWriter, r *http.Request) {
		fmt.Fprint(w, "Working using Go 1.17")
	})

	http.HandleFunc("/flights/", func(w http.ResponseWriter, r *http.Request) {
		toSend, err := getFlights()
		// TODO handle erros for the client bad request
		if err != nil {
			w.WriteHeader(http.StatusInternalServerError)
			fmt.Fprint(w, err)
			return
		}

		fmt.Fprint(w, string(toSend))
	})

	http.HandleFunc("/book/", func(w http.ResponseWriter, r *http.Request) {
		body, err := ioutil.ReadAll(r.Body)
		// TODO handle erros for the client bad request
		if err != nil {
			w.WriteHeader(http.StatusInternalServerError)
			fmt.Fprint(w, err)
			return
		}

		var recvCmd commande
		err = json.Unmarshal(body, &recvCmd)
		if err != nil {
			// TODO better handle
			w.WriteHeader(http.StatusBadRequest)
			fmt.Fprint(w, err)
			return
		}

		err = bookFlights(recvCmd)
		if err != nil {
			// TODO better handle
			w.WriteHeader(http.StatusBadRequest)
			fmt.Fprint(w, err)
			return
		}

		fmt.Fprint(w, "Ok 200")

	})

	fmt.Println("Server started")
	log.Fatal(http.ListenAndServe("0.0.0.0:7864", nil))
}

type flight struct {
	Code            string `json:"code"`
	From            string `json:"from"`
	To              string `json:"to"`
	AvailablePlaces int64  `json:"available_places"`
	Price           int64  `json:"price"`
}

type recvFlight struct {
	Code            string `json:"flightCode"`
	From            string `json:"from"`
	To              string `json:"to"`
	AvailablePlaces int64  `json:"availablePlaces"`
	Price           int64  `json:"price"`
}

func getFlights() ([]byte, error) {
	// Get flight from backend
	resp, err := http.Get(fmt.Sprintf("%s/api/flight/GetOurFlights", backURL))
	if err != nil {
		return nil, err
	}

	if resp.StatusCode > 299 && resp.StatusCode < 200 {
		return nil, fmt.Errorf("Failed to get flight. Server respond with status: %s", resp.Status)
	}

	bytesBody, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		return nil, err
	}

	var givenFlights []recvFlight
	err = json.Unmarshal(bytesBody, &givenFlights)
	if err != nil {
		return nil, err
	}

	flights := make([]flight, len(givenFlights))
	for i, f := range givenFlights {
		flights[i] = flight{
			Code:            f.Code,
			From:            f.From,
			To:              f.To,
			AvailablePlaces: f.AvailablePlaces,
			Price:           f.Price,
		}
	}

	return json.Marshal(flights)
}

type commande struct {
	Fname       string   `json:"fname"`
	Lname       string   `json:"lname"`
	Nat         string   `json:"nat"`
	FlightCodes []string `json:"flight_codes"`
	LoungeSupp  bool     `json:"lounge_supplement"`
	Currency    string   `json:"currency"`
}

type sendCmd struct {
	Fname       string              `json:"FirstName"`
	Lname       string              `json:"LastName"`
	Nat         string              `json:"Nationality"`
	FlightCodes []map[string]string `json:"FlightCodes"`
	LoungeSupp  bool                `json:"LoungeSupplement"`
	Currency    string              `json:"Currency"`
}

func bookFlights(cmd commande) error {
	flightCodes := make([]map[string]string, len(cmd.FlightCodes))
	for i, flyCde := range cmd.FlightCodes {
		flightCodes[i] = map[string]string{
			"code": flyCde,
		}
	}

	cmdToSend := sendCmd{
		Fname:       cmd.Fname,
		Lname:       cmd.Lname,
		Nat:         cmd.Nat,
		FlightCodes: flightCodes,
		LoungeSupp:  cmd.LoungeSupp,
		Currency:    cmd.Currency,
	}

	sendCtn, err := json.Marshal(cmdToSend)
	if err != nil {
		return err
	}

	resp, err := http.Post(fmt.Sprintf("%s/api/flight/bookTicket", backURL), "application/json", strings.NewReader(string(sendCtn)))

	if resp.StatusCode > 299 && resp.StatusCode < 200 {
		return fmt.Errorf("Failed to book flight. Server respond with status: %s", resp.Status)
	}

	return err
}
