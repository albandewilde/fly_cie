FROM golang:1.17 as builder

WORKDIR /usr/src/out
COPY . .

RUN CGO_ENABLED=0 go build -o /bin/out


FROM alpine

WORKDIR /bin/out

COPY --from=builder /bin/out .

CMD ["./out"]
