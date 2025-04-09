# ETAMP (Encrypted Token And Message Protocol)

## Overview

ETAMP (Encrypted Token And Message Protocol) is designed for secure and efficient message transmission in a
semi-decentralized network. The protocol ensures message integrity and supports encryption and signing using ECC (
Elliptic Curve Cryptography).

## Architecture

The protocol includes the following fields:

- `GUID Id`
- `double Version`
- `string Token` (compressed JSON)
- `string UpdateType`
- `string SignatureMessage`
- `string CompressionType`

### Token Structure

The token structure includes the following fields:

- `Guid Id`
- `Guid MessageId`
- `bool IsEncrypted`
- `string Data`
- `DateTimeOffset TimeStamp`

## Installation

Install the required ETAMP libraries using the NuGet Package Manager:

```shell
Install-Package ETAMP.Compression
Install-Package ETAMP.Core
Install-Package ETAMP.Encryption
Install-Package ETAMP.Extension.ServiceCollection
Install-Package ETAMP.Provider
Install-Package ETAMP.Validation
```

## Conclusion

This README provides an updated overview of the ETAMP protocol, including its usage with examples. Follow the structure
to integrate ETAMP into your projects for secure and efficient message handling.

## Contributing

Contributions to ETAMP are welcome! Please fork the repository, make your changes, and submit a pull request. We
appreciate your input in improving the project.

## License

ETAMP is licensed under the MIT License. For more information, see
the [LICENSE](https://github.com/max2020204/ETAMP/blob/master/LICENSE.txt) file in the repository.
