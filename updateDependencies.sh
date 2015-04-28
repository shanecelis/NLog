#!/bin/sh
./buildPackage.sh
if [ $? = 0 ]
then
	echo "*** UPDATING DEPENDENCIES"

	BIN_DIR="bin"

	NLOG="NLog"
	CMDTOOL=$NLOG".CommandLineTool"
	UNITY=$NLOG".Unity"

	UNITY_LIBS_DIR=$UNITY"/Assets/Libraries"

	echo "*** CLEAN"
	find "./"$UNITY_LIBS_DIR -type f -name "*.cs" -delete

	echo "*** COPY"
	cp -r $BIN_DIR"/"$NLOG $UNITY_LIBS_DIR

	echo "*** DONE ***"
else
	echo "ERROR: Tests didn't pass!"
	exit 1
fi
