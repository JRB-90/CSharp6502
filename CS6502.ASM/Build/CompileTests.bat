ECHO OFF
ECHO Beginning build

ca65 %~dp0\..\ASM\loadStoreTests.asm -o loadStoreTests.o
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to compile load/store tests
  EXIT ERROR
)

ca65 %~dp0\..\ASM\transferTests.asm -o transferTests.o
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to compile transfer tests
  EXIT ERROR
)

ca65 %~dp0\..\ASM\statusTests.asm -o statusTests.o
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to compile status tests
  EXIT ERROR
)

ca65 %~dp0\..\ASM\stackTests.asm -o stackTests.o
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to compile stack tests
  EXIT ERROR
)

ca65 %~dp0\..\ASM\subTests.asm -o subTests.o
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to compile subroutine tests
  EXIT ERROR
)

ca65 %~dp0\..\ASM\bitwiseTests.asm -o bitwiseTests.o
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to compile bitwise tests
  EXIT ERROR
)

ca65 %~dp0\..\ASM\mathTests.asm -o mathTests.o
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to compile math tests
  EXIT ERROR
)

ca65 %~dp0\..\ASM\incrementTests.asm -o incrementTests.o
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to compile increment tests
  EXIT ERROR
)

ca65 %~dp0\..\ASM\compareTests.asm -o compareTests.o
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to compile compare tests
  EXIT ERROR
)

ca65 %~dp0\..\ASM\branchTests.asm -o branchTests.o
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to compile branch tests
  EXIT ERROR
)


ld65 -C basic.cfg loadStoreTests.o -o %~dp0\..\Compiled\loadStoreTests.bin
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to link load/store tests
  EXIT ERROR
)

ld65 -C basic.cfg transferTests.o -o %~dp0\..\Compiled\transferTests.bin
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to link transfer tests
  EXIT ERROR
)

ld65 -C basic.cfg statusTests.o -o %~dp0\..\Compiled\statusTests.bin
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to link status tests
  EXIT ERROR
)

ld65 -C basic.cfg stackTests.o -o %~dp0\..\Compiled\stackTests.bin
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to link stack tests
  EXIT ERROR
)

ld65 -C basic.cfg subTests.o -o %~dp0\..\Compiled\subTests.bin
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to link subroutine tests
  EXIT ERROR
)

ld65 -C basic.cfg bitwiseTests.o -o %~dp0\..\Compiled\bitwiseTests.bin
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to link bitwise tests
  EXIT ERROR
)

ld65 -C basic.cfg mathTests.o -o %~dp0\..\Compiled\mathTests.bin
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to link math tests
  EXIT ERROR
)

ld65 -C basic.cfg incrementTests.o -o %~dp0\..\Compiled\incrementTests.bin
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to link increment tests
  EXIT ERROR
)

ld65 -C basic.cfg compareTests.o -o %~dp0\..\Compiled\compareTests.bin
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to link compare tests
  EXIT ERROR
)

ld65 -C basic.cfg branchTests.o -o %~dp0\..\Compiled\branchTests.bin
IF %ERRORLEVEL% NEQ 0 (
  ECHO Failed to link branch tests
  EXIT ERROR
)


ECHO Compile and linking ASM OK
ECHO Creating CPU cycle files

ECHO Creating load/store tests...
cpucheck.exe %~dp0\..\Compiled\loadStoreTests.bin %~dp0\..\Compiled\loadStoreTests.csv
ECHO Creating transfer tests...
cpucheck.exe %~dp0\..\Compiled\transferTests.bin %~dp0\..\Compiled\transferTests.csv
ECHO Creating status tests...
cpucheck.exe %~dp0\..\Compiled\statusTests.bin %~dp0\..\Compiled\statusTests.csv
ECHO Creating stack tests...
cpucheck.exe %~dp0\..\Compiled\stackTests.bin %~dp0\..\Compiled\stackTests.csv
ECHO Creating subroutine tests...
cpucheck.exe %~dp0\..\Compiled\subTests.bin %~dp0\..\Compiled\subTests.csv
ECHO Creating bitwise tests...
cpucheck.exe %~dp0\..\Compiled\bitwiseTests.bin %~dp0\..\Compiled\bitwiseTests.csv
ECHO Creating math tests...
cpucheck.exe %~dp0\..\Compiled\mathTests.bin %~dp0\..\Compiled\mathTests.csv
ECHO Creating increment tests...
cpucheck.exe %~dp0\..\Compiled\incrementTests.bin %~dp0\..\Compiled\incrementTests.csv
ECHO Creating compare tests...
cpucheck.exe %~dp0\..\Compiled\compareTests.bin %~dp0\..\Compiled\compareTests.csv
ECHO Creating branch tests...
cpucheck.exe %~dp0\..\Compiled\branchTests.bin %~dp0\..\Compiled\branchTests.csv

ECHO CPU cycle files created successfully
