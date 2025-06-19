FROM cgr.dev/chainguard/wolfi-base:latest@sha256:b72df108f3388c82b0638bcfbad1511d85c60593e67fb8f8a968255f7e0588df AS build

RUN apk add --no-cache \
    nodejs-24 \
    npm \
    chromium

WORKDIR /app
COPY package.json .
ENV PLAYWRIGHT_SKIP_BROWSER_DOWNLOAD=true
RUN npm install --omit=dev
COPY server.js .

RUN mkdir -p /runtime-root/usr/lib /runtime-root/usr/bin /runtime-root/app && \
    cp -a /usr/lib /runtime-root/usr/ && \
    cp -a /usr/bin /runtime-root/usr/ && \
    ln -sf chromium /runtime-root/usr/bin/chromium-browser && \
    cp -a /app/* /runtime-root/app && \
    rm -rf /runtime-root/usr/lib/node_modules /runtime-root/usr/lib/apk

RUN rm -f /runtime-root/usr/bin/busybox && \
    rm -f /runtime-root/usr/bin/npm || true && \
    rm -rf /runtime-root/usr/share/busybox \
           /runtime-root/usr/share/doc/busybox \
           /runtime-root/usr/lib/busybox* \
               /runtime-root/usr/share/npm \
               /runtime-root/usr/lib/node_modules/npm

FROM cgr.dev/chainguard/glibc-dynamic:latest@sha256:1157181f2a95aeb22cb2273f0ea8e4ba88dfc493e17513f26376af9555e37261 AS final

COPY --from=build /runtime-root/usr /usr
COPY --from=build /lib /lib
COPY --from=build /runtime-root/app /app

WORKDIR /app
USER 65532:65532

ENV LANG=C.UTF-8
EXPOSE 8080
CMD ["node", "server.js"]
