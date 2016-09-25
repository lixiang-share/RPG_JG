#/bin/bash
PROJECT_PATH=/home/giu/workspace/MyRPG/RPG_JG/RPG_Server/JWorkSpace/JServer
JAR_PATH=$PROJECT_PATH/lib
BIN_PATH=$PROJECT_PATH/bin
SRC_PATH=$PROJECT_PATH/src

#remmove some unuse fold
if [ -d $BIN_PATH ]; then
	rm -rf $BIN_APTH
else
	mkdir $BIN_PATH
fi

#copy some config file
cp $SRC_PATH/c3p0.properties $BIN_PATH
cp $SRC_PATH/log4j.properties $BIN_PATH
cp -r $PROJECT_PATH/config  $BIN_PATH

# First remove the sources.list file if it exists and then create the sources file of the project
find $SRC_PATH -name *.java > $SRC_PATH/sources.list
#find /home/giu/workspace/MyRPG_JG/RPG_Server/JWorkSpace/JServer -name *.java > $SRC_PATH/sources.list
# First remove the ONSServer directory if it exists and then create the bin directory of ONSServer
# Compile the project
javac -d $BIN_PATH -cp  "$JAR_PATH/*" @$SRC_PATH/sources.list -encoding gbk

