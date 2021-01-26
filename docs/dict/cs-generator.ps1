param (
	[string]$letter,
	[string]$rawData,
	[switch] $processAll
)

if($processAll){
	Get-ChildItem .\!Dictionary-in-csv\ `
	| ForEach-Object {
		 .\cs-generator.ps1 $_.BaseName ((Get-Content $_) -join "`n") > ".\!Dictionary-in-cs\EngDict$($_.BaseName).cs" }
} else {
	$DictTemplate =
"namespace Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDict$letter : DictionaryBase
    {
        protected override string WordsInCsv =>
@""$letter"";
    }
}
";
echo $DictTemplate
}
