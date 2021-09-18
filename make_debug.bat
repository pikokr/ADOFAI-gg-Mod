
mkdir tmp
cd tmp
mkdir ADOFAI-gg
cd ADOFAI-gg
mkdir lib
cd ..

copy ..\ADOFAI-GG\bin\Debug\ADOFAI_GG.dll ADOFAI-gg
xcopy ..\lib\dependancy\*.* ADOFAI-gg\lib\
xcopy ..\lib\dependancy\mono\*.* ADOFAI-gg\lib\

tar -a -c -f ADOFAI-gg-%1.zip ADOFAI-gg
move ADOFAI-gg-%1.zip ..
cd ..
rmdir /s /q tmp
