#!/bin/bash

# Set the Common Name (CN) for the certificate
cn="localhost"

# Generate the certificate
openssl req -x509 -newkey rsa:4096 -keyout key.pem -out cert.pem -days 365 -nodes -subj "//CN=$cn"

# Generate the .pfx file
openssl pkcs12 -export -out $cn.pfx -inkey key.pem -in cert.pem -password pass:YourSecurePassword


# Move the .pfx file to the certificates directory and remove the temporary pem files
mkdir -p SamlBlazorApp/certificates
mv $cn.pfx SamlBlazorApp/certificates/
rm key.pem cert.pem

echo "Certificate generated and moved to SamlBlazorApp/certificates/ directory."