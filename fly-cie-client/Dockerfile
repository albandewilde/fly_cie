FROM node as builder

WORKDIR /app

RUN npm install -g @angular/cli

COPY . /app

RUN yarn

RUN ng build


FROM caddy

COPY config/Caddyfile /etc/caddy/Caddyfile

COPY --from=builder /app/dist/fly-cie-client/* /srv/
