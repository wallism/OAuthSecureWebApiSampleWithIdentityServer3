."./New-SelfsignedCertificateEx.ps1"

New-SelfsignedCertificateEx -Subject "CN=www.mycompany.com" `
-EKU "Server Authentication" -KeySpec "Exchange" `
-Path ".\certs\testSsl.pfx"  `
-Password(ConvertTo-SecureString "CertSecurePassword" -AsPlainText -Force) `
-Exportable `
-KeyUsage "DigitalSignature" -FriendlyName "A dev test cert" `
-NotAfter $([datetime]::now.AddYears(5))