.PHONY: test start build-no-cache

start:
	@docker-compose up --build

build-no-cache:
	@docker-compose build --no-cache

test: build-no-cache
	@docker-compose run server python3 -m pytest
