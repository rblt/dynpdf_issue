# dynpdf_issue

This repository's only purpose is to demonstrate the Dynamic PDF Core Suite's
issue when signing document with TSA using an ECDSA certificate.


### Environment

- **OS version**: Windows 11
- **.NET version**: .NET Framework 4.8
- **Dynamic PDF Core Suite .NET version**: 12.22.0


### Build & run

The console application can be built with VS2022, but older version's may work.
After compiling the project, run the executable which will crash with the following:

```
Unhandled exception: System.NotSupportedException: The certificate key algorithm is not supported.
   at System.Security.Cryptography.X509Certificates.X509Certificate2.get_PrivateKey()
   at ceTe.DynamicPDF.Certificate.#kJ(Byte[] #xmc, #UKe #ymc)
   at #6lc.#2uc.#kJ(Byte[] #xmc, #UKe #ymc)
   at ceTe.DynamicPDF.Document.Draw(Stream stream)
   at ceTe.DynamicPDF.Document.Draw(String filePath)
   at DynamicPDF_TSA_Issue.Program.Main(String[] args) in D:\Projects\_FORNAX_\DynamicPDF_TSA_Issue\Program.cs:line 27
```

### Certificate details

The project is using a seld-signed certificate with Elliptic Curve Digital Signiture Algorithm (ECDSA)
using 384 bits private key.

The certificate was generated with OpenSSL, using the following commands:

1. Generate the 384 bits private key using secp384r1 named curve:
```bat
openssl ecparam -name secp384r1 -genkey -noout -out ec_private_key_384.pem
```
2. Create the certificate signing request (CSR):
```bat
openssl req -new -key ec_private_key_384.pem -out ec_csr.pem
```
3. Generate the self-signed certificate:
```bat
openssl req -x509 -key ec_private_key_384.pem -in ec_csr.pem -out ec_certificate_384.pem -days 365
```
4. Pack the private key and certificate into a PKCS#12 (.pfx) file with a tool like _KeyStoreExplorer_.


### Assumed error

Inspecting the exception's stack trace, the DynamicPDF Core Suite library calls the `X509Certificate2` class'
`PrivateKey` property which is an obsolete method, instead, `GetRSAPrivateKey()`, `GetDSAPrivateKey()`, or
`GetECDsaPrivateKey()` should be called.


