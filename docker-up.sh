#!/bin/bash

docker-compose -f "docker-compose.yml" -f "docker-compose.$1.yml" down
docker-compose -f "docker-compose.yml" -f "docker-compose.$1.yml" up -d --build --force-recreate --remove-orphans
