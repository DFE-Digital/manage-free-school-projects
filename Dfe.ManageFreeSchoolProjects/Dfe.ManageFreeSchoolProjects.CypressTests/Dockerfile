FROM cypress/base:20.18.0

COPY ./cypress ./cypress
COPY cypress.config.ts .
COPY package-lock.json package-lock.json
COPY package.json package.json
COPY tsconfig.json .

RUN npm install
ENTRYPOINT ["npm", "run", "cy:zap"]