FROM kristophjunge/test-saml-idp

RUN mkdir -p /var/simplesamlphp/cert && \
    openssl req -x509 -newkey rsa:4096 -keyout /var/simplesamlphp/cert/server.pem -out /var/simplesamlphp/cert/server.crt -days 365 -nodes -subj "/CN=localhost"


COPY authsources.php /var/www/simplesamlphp/config/authsources.php