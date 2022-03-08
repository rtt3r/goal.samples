export DOCKER_BUILDKIT=1
export COMPOSE_DOCKER_CLI_BUILD=1

.EXPORT_ALL_VARIABLES: ;

KELLYBOT_APPS=elasticsearch kibana api

.PHONY: build
build:
	docker-compose -f docker-compose.yml -f docker-compose.override.yml build --force-rm

.PHONY: run-dev
run-dev:
	docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build --force-recreate --remove-orphans
	# ./scripts/wait.sh

.PHONY: stop
stop:
	docker-compose stop

.PHONY: down
down:
	docker-compose down

.PHONY: clean
clean:
	docker-compose down -v

.PHONY: restart-dev
restart-dev:
	make clean
	make run-dev

.PHONY: remove
remove:
	make clean
	docker-compose rm -s -f ${KELLYBOT_APPS}

.PHONY: run-apps
run-apps:
	docker-compose -f docker-compose.yml -f "docker-compose.prod.yml" up -d --build --force-recreate --remove-orphans
	# ./scripts/wait.sh

.PHONY: run
run:
	make run-apps
