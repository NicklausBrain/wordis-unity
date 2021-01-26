param(
	[string] $dictPath = "./Dictionary-in-csv/M.csv",
	[switch] $processAll,
	[string] $outDir = "./!Dictionary-in-csv"
)

if($processAll){
	Get-ChildItem '.\Dictionary-in-csv\' `
	| ForEach-Object { $_ } `
	| ForEach-Object {
		.\Preprocessor.ps1 -dictPath $_.FullName > "$outDir\$($_.Name)" }
} else {
	Get-Content $dictPath `
	| ForEach-Object { $_.Trim('"').Trim() } `
	| Where-Object { 
		-not [string]::IsNullOrEmpty($_) `
		-and [char]::IsLetter($_[0]) }
}
