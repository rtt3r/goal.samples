#!/bin/bash

export DOCKER_DEFAULT_PLATFORM=linux/amd64

docker-compose down
docker-compose -f "docker-compose.yml" -f "docker-compose.$1.yml" up -d --build --force-recreate --remove-orphans
