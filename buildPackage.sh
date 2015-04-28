#!/bin/sh
./runTests.sh
if [ $? = 0 ]
then
	echo "*** BUILDING PACKAGE"

	BIN_DIR="bin"
	TMP_DIR=$BIN_DIR"/tmp"

	NLOG="NLog"
	CMDTOOL=$NLOG".CommandLineTool"
	UNITY=$NLOG".Unity"
	CMDTOOL_DIR=$TMP_DIR"/"$CMDTOOL

	xbuild /p:Configuration=Release $CMDTOOL"/"$CMDTOOL.csproj /verbosity:minimal

	echo "*** CLEAN"
	rm -rf $BIN_DIR

	echo "*** CREATE FOLDERS"
	mkdir $BIN_DIR
	mkdir $TMP_DIR
	mkdir $CMDTOOL_DIR

	echo "*** COPY SOURCES"
	cp -r $NLOG"/"$NLOG $TMP_DIR
	cp -r $CMDTOOL"/bin/Release/"{"CommandLineTool.exe","NLog.dll"} $CMDTOOL_DIR
	mv $CMDTOOL_DIR"/CommandLineTool.exe" $CMDTOOL_DIR"/nlog.exe"
	cp -r $UNITY"/Assets/"$UNITY $TMP_DIR
	cp README.md ${TMP_DIR}/README.md

	echo "*** DELETE GARBAGE"
	find "./"$TMP_DIR -name "*.meta" -type f -delete
	find "./"$TMP_DIR -name "*.DS_Store" -type f -delete

	echo "*** CREATE ZIP ARCHIVE"
	cd $TMP_DIR
	zip -rq ../NLog.zip ./
	cd -

	echo "*** COPY TEMP TO BIN"
	cp -r $TMP_DIR"/." $BIN_DIR

	echo "*** DELETE TEMP FOLDER"
	rm -rf $TMP_DIR

	echo "*** DONE ***"
else
	echo "ERROR: Tests didn't pass!"
	exit 1
fi
