**Remake classic Set Default Programs page from Default Programs Control Panel (sud.dll) for Windows 10 1809 and above (x64) and Windows 11**

![Screenshot (83)](https://github.com/MehranAkbarii/DefaultProgramsRemake/assets/133998536/3cc0e1ca-5aca-4a50-adbe-2f11321a0ac9)

![Screenshot (84)](https://github.com/MehranAkbarii/DefaultProgramsRemake/assets/133998536/f0827a97-2688-4448-8abd-286349b2b957)


**Notes:**

Third party OpenWith menu replacment programs like OpenWithEnchanced are not compatible with this app and you may not able to change your default apps using this app because of possible conflicts.

Set Default Programs page from Default Programs (sud.dll) (Removed in Windows 10 1709-1803) is the only control panel page from windows 8 , 8.1 era that cannot be restored and cannot be remade completely and even there is no good replacement application for it even I could not remake it functionalities completely "Set default app or program for file types by app." feature was the only thing that I could remake it

These 3 functionalities are still missing (last two functionalities even missing in ms-settings page and completely removed from windows):

-Set default app or program for protocols by app. (there is no way to change protocols associations without using ms-settings or classic Default Programs\Set Associations page from Default Programs Control Panel because Windows uses hashes to prevent changing protocols associations by directly editing registry values ​​by users and third-party apps, Hash value data and ProgID value data are not changeable directly ​​by users and third-party apps)

-Make an app or a program the default for all file types and protocols it can open at once.

-Make an app or a program the default for multiple file types or protocols it can open at once.

I can not add anymore functionalities, if you face any issues tell me If I could I will fix it , since I used windows forms it is hard to me to make the app UI exact same as original page because windows forms is so limited, maybe in future I would remake the project using wpf and make app UI more look like original one

**Misc:**

To restore classic **Default Programs\Set Associations page** from Default Programs Control Panel download modified version of windows 10 1703 (x64) sud.dll and sud.dll.mui files from [here](https://github.com/MehranAkbarii/DefaultProgramsRemake/files/15055799/Modified_Windows10_1703_sud.dll.zip) and then copy and replace sud.dll to %systemroot%\System32 and sud.dll.mui file to %systemroot%\System32\en-US directories, **_dont forget to take a backup of your original sud.dll and sud.dll.mui files before copy and replace the files_**
