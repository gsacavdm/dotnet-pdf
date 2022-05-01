# How To Run
The simplest execution takes a file and prints out each line of text
```
./Sc.Pdf -f pdfs/my-file.pdf
```

The more specialized execution modes are used for parsing insurance EoBs and renaming the downloaded EoBs which have random file names to file names following my naming convention
```
./Sc.Pdf -d $(pwd) -m RegenceMove
```

# Helper Scripts

## Bash

### Compare Files Between Two Directories
This script takes two directories and compares the list of files in each. It only compares which files exist on each folder and does NOT compare the contents of the files. This script is helpful when processing insurance EoBs, to know which of the newly downloaded and renamed files are new.
```
sourcePath=
oneDrivePath=

diff <(find $sourcePath -exec basename {} \; | sort) <(find $oneDrivePath -exec basename {} \; | sort) --unchanged-line-format="BOTH: %L" --old-line-format="MISSING: %L" --new-line-format=""
```

Append this line at the end of the previous script to only copy the new files (versus all).
```
| grep MISSING | cut -d" " -f2- | xargs -I {} cp {} $oneDrivePath
```

## Powershell

### Dedup Regence Claims
This script removes duplicate Regence claims resulting from having files that follow the old naming convention and the new naming convention. The script gets all the files with the new naming convention and deletes their old naming convention equivalent if it exists. This approach ensures that we don't delete files with the old naming convention that don't have corresponding files with the new naming convention.
```
$files = gci -filter *Regence` E* | Select -ExpandProperty Name

foreach ($file in $files) {
  $fileParts = $file.Split(" ")
  $length = $fileParts.Length

  $firstParts = $fileParts | Select -First ($length-2)
$firstPart = [string]::Join(" ", $firstParts)

  $lastParts = $fileParts | Select -Last 1
  $lastPart = [string]::Join(" ", $lastParts)

  $dupFile = $firstPart + " " + $LastPart
  $dupFile

  rm $dupFile
}
```

