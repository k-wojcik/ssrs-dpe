$binFolder = "..\src\SSRSDataProcessingExtensions\bin\Release"

$PrivateAssembliesFolder = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\PrivateAssemblies\"
$RSDesignerConfig = Join-Path $PrivateAssembliesFolder RSReportDesigner.config
$RSPreviewPolicy = Join-Path $PrivateAssembliesFolder RSPreviewPolicy.config

Copy-Item -Force (Join-Path $binFolder SSRSDataProcessingExtensions.dll) $PrivateAssembliesFolder
Copy-Item -Force (Join-Path $binFolder SSRSDataProcessingExtensions.pdb) $PrivateAssembliesFolder

if(Test-path $RSDesignerConfig)
{	
	$RSDesignerConfigXml = [xml] (Get-Content $RSDesignerConfig) 
	$rootReporting = $RSDesignerConfigXml.Configuration.Extensions.Data;

	if($rootReporting.Extension | Where-Object {$_.Name -eq "REST_JSON"})
    {		
		Write-Host "DPE REST_JSON already added to the RSReportDesigner.config" -ForegroundColor Green	
	}
	else
	{	
		Write-Host "Creating backup RSReportDesigner.config file" -NoNewline
		Copy-Item $RSDesignerConfig ($RSDesignerConfig + ".bak")
		Write-Host "Done" -ForegroundColor Green
	
		$dpe = $RSDesignerConfigXml.CreateElement("Extension");
		$dpe.SetAttribute("Name","REST_JSON")
		$dpe.SetAttribute("Type","SSRSDataProcessingExtensions.JsonDPE.Extension.JsonConnection, SSRSDataProcessingExtensions")
		[Void]$rootReporting.AppendChild($dpe);
		
		$dpe2 = $RSDesignerConfigXml.CreateElement("Designer");
		$dpe2.SetAttribute("Name","REST_JSON")
		$dpe2.SetAttribute("Type","Microsoft.ReportingServices.QueryDesigners.GenericQueryDesigner, Microsoft.ReportingServices.QueryDesigners")
		[Void]$rootReporting.AppendChild($dpe2);
				
		$RSDesignerConfigXml.Save($RSDesignerConfig);	
        Write-Host "DPE REST_JSON added into RSReportDesigner.config" -ForegroundColor Green
	}
}
else
{
    Write-Host "RSReportDesigner.config not found: " -ForegroundColor Red
    Write-Host $RSDesignerConfig
}

$NewtonsoftJsonAssembly = (Join-Path $binFolder Newtonsoft.Json.dll)
if (-not (Test-Path $NewtonsoftJsonAssembly))
{
    Throw "Assembly file: Newtonsoft.Json.dll not found"
}
[Reflection.Assembly]::LoadWithPartialName("System.EnterpriseServices") | Out-Null
[System.EnterpriseServices.Internal.Publish] $publish = new-object System.EnterpriseServices.Internal.Publish
$publish.GacInstall($NewtonsoftJsonAssembly)
$NewtonsoftJsonGACAssembly = [reflection.assembly]::LoadWithPartialName("Newtonsoft.Json")
if ($NewtonsoftJsonGACAssembly)
{
	Write-Host "Newtonsoft.Json.dll assembly installed in GAC" -ForegroundColor Green
}else {
	Throw "Newtonsoft.Json.dll assembly is not installed in GAC"
}