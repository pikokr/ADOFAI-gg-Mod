
mkdir tmp
cd tmp
mkdir ADOFAI-gg
cd ADOFAI-gg
mkdir lib
cd ..

copy ..\ADOFAI-GG\bin\Debug\ADOFAI_GG.dll ADOFAI-gg
copy ..\ADOFAI-GG\bin\Debug\UniRx.dll ADOFAI-gg\lib
copy ..\ADOFAI-GG\bin\Debug\UniTask.Addressables.dll ADOFAI-gg\lib
copy ..\ADOFAI-GG\bin\Debug\UniTask.dll ADOFAI-gg\lib
copy ..\ADOFAI-GG\bin\Debug\UniTask.DOTween.dll ADOFAI-gg\lib
copy ..\ADOFAI-GG\bin\Debug\UniTask.Linq.dll ADOFAI-gg\lib
copy ..\ADOFAI-GG\bin\Debug\UniTask.TextMeshPro.dll ADOFAI-gg\lib

tar -a -c -f ADOFAI-gg-%1.zip ADOFAI-gg
move ADOFAI-gg-%1.zip ..
cd ..
rmdir /s /q tmp
