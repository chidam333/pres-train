FROM node:24-alpine3.21
WORKDIR /app
COPY package*.json .
RUN npm install
RUN npm install -g @angular/cli
COPY . .
RUN ng build --configuration production
RUN npm install -g http-server
RUN rm -rf node_modules
EXPOSE 4200
CMD ["http-server", "dist/frontend/browser", "-p", "4200"]