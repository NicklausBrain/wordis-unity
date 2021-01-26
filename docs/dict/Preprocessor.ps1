param(
	[string] $dictPath = "./Dictionary-in-csv/M.csv"
)

Get-Content $dictPath `
| ForEach-Object { $_.Trim('"').Trim() } `
| Where-Object { 
	-not [String]::IsNullOrEmpty($_) }