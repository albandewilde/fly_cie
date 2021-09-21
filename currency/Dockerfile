FROM python

WORKDIR /usr/src/app

RUN python3 -m pip install --upgrade pip

COPY requirements.txt .
RUN python3 -m pip install -r requirements.txt

COPY . .

CMD ["python3", "serveur.py"]
