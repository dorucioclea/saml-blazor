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
];