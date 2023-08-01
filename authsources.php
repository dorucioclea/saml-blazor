<?php
$config = [
    'admin' => [
        'core:AdminPassword',
    ],

    'example-userpass' => [
        'exampleauth:UserPass',
        'admin:adminpass' => [
            'uid' => ['admin'],
            'eduPersonAffiliation' => ['group1', 'group2'],
            'role' => ['admin'],
        ],
        'jdoe:jdpass' => [
            'uid' => ['jdoe'],
            'eduPersonAffiliation' => ['group1'],
            'role' => ['user'],
        ],
    ],

    'default-sp' => [
        'saml:SP',
        'privatekey' => 'cert/server.pem',
        'certificate' => 'cert/server.crt',
        'signature.algorithm' => 'http://www.w3.org/2001/04/xmldsig-more#rsa-sha256',
        'entityID' => 'saml-poc',
        'idp' => 'http://localhost:8080/simplesaml/saml2/idp/metadata.php',
        'assertion.encryption' => false,
    ],
];