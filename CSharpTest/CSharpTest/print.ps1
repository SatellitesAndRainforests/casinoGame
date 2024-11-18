 $directories = @(
 '.\Application', 
 '.\Configuration', 
 '.\Interfaces', 
 '.\Ui'
 )
 foreach ($dir in $directories) {
	     Get-ChildItem -Path $dir -Recurse -File | ForEach-Object {
		             Write-Host "`n---- Contents of: $($_.FullName) ----`n"
			             Get-Content $_.FullName
				         }
 }

