# Setup script for NSIS v2.46+ (Nullsoft Scriptable Install System - http://nsis.sourceforge.net)
!include WinVer.nsh

# define title of the setup
Name "LetsMT! Machine Translation Provider"

# define installer name
outFile "LetsMTProvider.exe"

# run as user, no UAC
RequestExecutionLevel user

# set the install directory
Function .onInit
${If} ${AtLeastWinVista}
# set local appdata folder as install directory  on Vista+
StrCpy $INSTDIR "$LOCALAPPDATA\SDL\SDL Trados Studio\9\Plugins\Packages"
${Else}
# set appdata folder as install directory 
StrCpy $INSTDIR "$APPDATA\..\Local Settings\Application Data\SDL\SDL Trados Studio\9\Plugins\Packages"
${EndIf}
FunctionEnd 

# The text to prompt the user to enter a directory
DirText "Please select SDL Trados plugin directory"

# default section start
section
 
# define output path
setOutPath $INSTDIR
 
# specify file to go in output path
file "C:\Users\karlis.gedins\Documents\Visual Studio 2010\Projects\LetsMT!\SDLTradosPlugin\LetsMT.MTProvider.sdlplugin"
 
# define uninstaller name
writeUninstaller $INSTDIR\uninstaller.exe

CreateDirectory "$SMPROGRAMS\LetsMT! Machine Translation Provider"
CreateShortCut "$SMPROGRAMS\LetsMT! Machine Translation Provider\Uninstall.lnk" "$INSTDIR\uninstaller.exe"
 
# default section end
sectionEnd
 
# create a section to define what the uninstaller does.
# the section will always be named "Uninstall"
section "Uninstall"
 
# Always delete uninstaller first
delete $INSTDIR\uninstaller.exe
 
# now delete installed file
delete $INSTDIR\LetsMT.MTProvider.sdlplugin


delete $INSTDIR\..\Unpacked\LetsMT.MTProvider\LetsMT.MTProvider.dll
delete $INSTDIR\..\Unpacked\LetsMT.MTProvider\LetsMT.MTProvider.plugin.resources
delete $INSTDIR\..\Unpacked\LetsMT.MTProvider\LetsMT.MTProvider.plugin.xml
delete $INSTDIR\..\Unpacked\LetsMT.MTProvider\pluginpackage.manifest.xml
RMDIR $INSTDIR\..\Unpacked\LetsMT.MTProvider\

Delete "$SMPROGRAMS\LetsMT! Machine Translation Provider\Uninstall.lnk"
RMDIR "$SMPROGRAMS\LetsMT! Machine Translation Provider"
 
sectionEnd