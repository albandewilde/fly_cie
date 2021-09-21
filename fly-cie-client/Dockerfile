FROM node as builder

RUN npm install -g @angular/cli

WORKDIR /app

COPY . .

RUN yarn

CMD ng serve --host 0.0.0.0 --port 7862
