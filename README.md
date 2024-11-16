## Remake classic Set Default Programs page from Default Programs Control Panel (sud.dll) for Windows 10 1809 and above (x64) and Windows 11

![Screenshot (83)](https://github.com/MehranAkbarii/DefaultProgramsRemake/assets/133998536/3cc0e1ca-5aca-4a50-adbe-2f11321a0ac9)

![Screenshot (84)](https://github.com/MehranAkbarii/DefaultProgramsRemake/assets/133998536/f0827a97-2688-4448-8abd-286349b2b957)

![Screenshot (120)](https://github.com/user-attachments/assets/d1578433-a159-49cc-8d06-16ccc22e53aa)

![Screenshot (119)](https://github.com/user-attachments/assets/b7233593-abd6-4b40-b9fb-281f75b4c716)

![Screenshot (118)](https://github.com/user-attachments/assets/bf0799ff-5f8d-4fb1-9265-d9834ba1f83d)


## Notes

Third party OpenWith menu replacment programs like OpenWithEnchanced are not compatible with this app and you may not able to change default apps using this app because of possible conflicts.

Set Default Programs page from Default Programs (sud.dll) (Removed in Windows 10 1709-1803) is the only control panel page from windows 8 , 8.1 era that cannot be restored and cannot be remade completely and even there is no good replacement application for it even I could not remake it functionalities completely "Set default app or program for file types by app." feature was the only thing that I could remake it

These 3 functionalities are still missing (first two functionalities even missing in ms-settings page and completely removed from windows):

- Make an app or a program the default for all file types and protocols it can open at once.

- Make an app or a program the default for multiple file types or protocols it can open at once.

- Set non browser win32 programs as default programs for protocols by app. <blockquote>

The reason why I can remake this functionality because there is no way to change protocols associations without using ms-settings or classic Default Programs\Set Associations page from Default Programs Control Panel because Windows uses hashes to prevent changing protocols associations by directly editing registry values ​​by users and third-party apps, Hash value data and ProgID value data are not changeable directly ​​by users and third-party apps 

**Update 06/10/2024:** from now you can set UWP apps and your installed web browsers as default programs for the protocols that their support using this app, this app do it by help of [SetUserFTA](https://kolbi.cz/blog/2017/10/25/setuserfta-userchoice-hash-defeated-set-file-type-associations-per-user/) in background

![Screenshot (116)](https://github.com/user-attachments/assets/dc66ea7b-dcf0-4255-98f9-63746eb3723e)


I don't know how can I get list of supported protocols for non-UWP apps and non browser win32 programs in my app so supported protocols for non-UWP apps and non browser win32 programs won't be listed  

![Screenshot (117)](https://github.com/user-attachments/assets/2eb4ab17-5df2-46a9-8b5d-30dfd80998a9)


</blockquote>

## Contribution:

I need contribution to complete this app, if someone has knowledge about it please contribute, I can not add anymore functionalities, if you face any issues tell me If I could I will fix it , since I used windows forms it is hard to me to make the app UI exact same as original page because windows forms is so limited, maybe in future I would remake the project using wpf and make app UI more look like original one

## Known issues: 

-The openWith menu will not open when Windows system language is set to a language other than English (I won't fix it because I don't care about localization, so set english as your windows display language if you want this app work correctly on your system)

-Some UWP apps and Win32 programs with file or protocol opening capabilities won't be listed in apps listview

-Names of some current default apps of some protocols or extensions won't be listed in current defaults column of extentions listview and will listed as Unknown Application

## Misc:

To restore classic **Default Programs\Set Associations page** from Default Programs Control Panel download modified version of windows 10 1703 (x64) sud.dll and sud.dll.mui files from [here](https://github.com/MehranAkbarii/DefaultProgramsRemake/files/15055799/Modified_Windows10_1703_sud.dll.zip) and then copy and replace sud.dll to %systemroot%\System32 and sud.dll.mui file to %systemroot%\System32\en-US directories, **_dont forget to take a backup of your original sud.dll and sud.dll.mui files before copy and replace the files_**
