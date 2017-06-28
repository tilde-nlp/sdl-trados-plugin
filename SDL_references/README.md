## SDL Trados build dependencies

During the build process this folder get's populated with SDL Trados
library files taken from the appropriate version of SDL Trados Studio
SDK. This is done by the `CopyReferences` task in the `BeforeBuild`
build target which takes the references from the appropriate SDK
directory according to the current build configuration name
(v2010\_Debug, v2010\_Release, v2014\_Debug, etc.). See the
[ReferencesCopy.targets](../Custom_targets/ReferencesCopy.targets)
file for more details.

If the appropriate SDL Trados SDK version is not installed when
building a specific configuration, then the build fails with
an arror message.

For convenience, the Trados 2015/2017 library files are included in
the [Custom\_targets.back](../Custom_targets.back) folder. To use
them you should probably copy them to the appropriate SDK location.
Check the
[ReferencesCopy.targets](../Custom_targets/ReferencesCopy.targets)
file for SDK paths.
