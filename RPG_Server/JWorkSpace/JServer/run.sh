#/bin/bash

ROOT=$PWD
nohup java -classpath "$ROOT/bin:$ROOT/lib/*" jg.rpg.LaunchServer &

