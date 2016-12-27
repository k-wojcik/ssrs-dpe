$binFolder = "..\src\SSRSDataProcessingExtensions\bin\Release"

if (Test-Path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\MSRS13.MSSQLSERVER\Setup")
{
    $SSRSInstallFolder = (Get-ItemProperty -path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\MSRS13.MSSQLSERVER\Setup").SQLpath
}

$RSBinFolder = "$SSRSInstallFolder" + "ReportServer\bin\"
$RSReportServerConfig = "$SSRSInstallFolder" + "ReportServer\rsreportserver.config"
$RSSrvPolicyConfig = "$SSRSInstallFolder" + "ReportServer\rssrvpolicy.config"

Copy-Item -Force (Join-Path $binFolder SSRSDataProcessingExtensions.dll) $RSBinFolder
Copy-Item -Force (Join-Path $binFolder SSRSDataProcessingExtensions.pdb) $RSBinFolder
Copy-Item -Force (Join-Path $binFolder Newtonsoft.Json.dll) $RSBinFolder

if(Test-path $RSReportServerConfig)
{	
	$RSReportingServerConfigXml = [xml] (Get-Content $RSReportServerConfig) 
	$rootReporting = $RSReportingServerConfigXml.Configuration.Extensions.Data;

	if($rootReporting.Extension | Where-Object {$_.Name -eq "REST_JSON"})
    {		
		Write-Host "DPE REST_JSON already added to the rsreportserver.config" -ForegroundColor Green	
	}
	else
	{	
		Write-Host "Creating backup rsreportserver.config file" -NoNewline
		Copy-Item $RSReportServerConfig ($RSReportServerConfig + ".bak")
		Write-Host "Done" -ForegroundColor Green
	
		$dpe = $RSReportingServerConfigXml.CreateElement("Extension");
		$dpe.SetAttribute("Name","REST_JSON")
		$dpe.SetAttribute("Type","SSRSDataProcessingExtensions.JsonDPE.Extension.JsonConnection, SSRSDataProcessingExtensions")
		[Void]$rootReporting.AppendChild($dpe);
		$RSReportingServerConfigXml.Save($RSReportServerConfig);
        Write-Host "DPE REST_JSON added into rsreportserver.config" -ForegroundColor Green
	}
}
else
{
    Write-Host "rsreportserver.config not found: " -ForegroundColor Red
    Write-Host $RSReportServerConfig
}

if(Test-path $RSSrvPolicyConfig)
{	
	$RSSrvPolicyConfigXml = [xml] (Get-Content $RSSrvPolicyConfig) 
	$rootReporting = $RSSrvPolicyConfigXml.configuration.mscorlib.security.policy.PolicyLevel;

	if(!($rootReporting.CodeGroup | Where-Object {$_.Name -eq "DPE_REST_JSON"}) -Or !($rootReporting.CodeGroup | Where-Object {$_.Name -eq "DPE_REST_JSON_Newtonsoft_Json"}))
    {		
		Write-Host "Creating backup rssrvpolicy.config file" -NoNewline
		Copy-Item $RSSrvPolicyConfig ($RSSrvPolicyConfig + ".bak")
		Write-Host "Done" -ForegroundColor Green
	}
	
	if($rootReporting.CodeGroup | Where-Object {$_.Name -eq "DPE_REST_JSON"})
    {		
		Write-Host "DPE_REST_JSON already added to the rssrvpolicy.config" -ForegroundColor Green	
	}
	else
	{		    
		$dpe = $RSSrvPolicyConfigXml.CreateElement("CodeGroup");
		$dpe.SetAttribute("Name","DPE_REST_JSON")
		$dpe.SetAttribute("class","UnionCodeGroup")
		$dpe.SetAttribute("version","1")
		$dpe.SetAttribute("PermissionSetName","FullTrust")
		[Void]$rootReporting.AppendChild($dpe);
		
		$rootGroup = (($rootReporting.CodeGroup | where {$_.Name -eq "DPE_REST_JSON"}))
		$dpe2 = $RSSrvPolicyConfigXml.CreateElement("IMembershipCondition");
		$dpe2.SetAttribute("class","UrlMembershipCondition")
		$dpe2.SetAttribute("version","1")
		$dpe2.SetAttribute("Url", $RSBinFolder + "SSRSDataProcessingExtensions.dll")
		[Void]$rootGroup.AppendChild($dpe2);
		
		$RSSrvPolicyConfigXml.Save($RSSrvPolicyConfig);
        Write-Host "DPE_REST_JSON added into rssrvpolicy.config" -ForegroundColor Green
	}
	
	if($rootReporting.CodeGroup | Where-Object {$_.Name -eq "DPE_REST_JSON_Newtonsoft_Json"})
    {		
		Write-Host "DPE_REST_JSON_Newtonsoft_Json already added to the rssrvpolicy.config" -ForegroundColor Green	
	}
	else
	{		
		$dpe = $RSSrvPolicyConfigXml.CreateElement("CodeGroup");
		$dpe.SetAttribute("Name","DPE_REST_JSON_Newtonsoft_Json")
		$dpe.SetAttribute("class","UnionCodeGroup")
		$dpe.SetAttribute("version","1")
		$dpe.SetAttribute("PermissionSetName","FullTrust")
		[Void]$rootReporting.AppendChild($dpe);
		
		$rootGroup = (($rootReporting.CodeGroup | where {$_.Name -eq "DPE_REST_JSON_Newtonsoft_Json"}))
		$dpe2 = $RSSrvPolicyConfigXml.CreateElement("IMembershipCondition");
		$dpe2.SetAttribute("class","UrlMembershipCondition")
		$dpe2.SetAttribute("version","1")
		$dpe2.SetAttribute("Url", $RSBinFolder + "Newtonsoft.Json.dll")
		[Void]$rootGroup.AppendChild($dpe2);
		
		$RSSrvPolicyConfigXml.Save($RSSrvPolicyConfig);
        Write-Host "DPE added into rssrvpolicy.config" -ForegroundColor Green
	}
}
else
{
    Write-Host "rssrvpolicy.config not found: " -ForegroundColor Red
    Write-Host $RSSrvPolicyConfig
}
