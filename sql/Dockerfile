FROM postgres
WORKDIR /app
EXPOSE 5432
ENV POSTGRES_USER=sa
ENV POSTGRES_PASSWORD=samplepass
COPY seedcrypto.sql /docker-entrypoint-initdb.d/