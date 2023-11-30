# Use Node.js LTS version as base image
FROM node:18.17.0-alpine as builder

# Set the working directory in the container
WORKDIR /app

# Copy package.json and package-lock.json to the working directory
COPY package.json package-lock.json ./

RUN npm install -g npm@latest

# Install dependencies
RUN npm install --legacy-peer-deps

# Copy the entire Angular app to the working directory
COPY . .

# Build the Angular app
RUN npm run build --  --output-path=dist --output-hashing=all

# Use Nginx as the production server
FROM nginx:stable-alpine

# Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*

COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf

# Copy dist folder fro build stage to nginx public folder
COPY --from=builder /app/dist/* /usr/share/nginx/html/

# fix folder structure
RUN rm /usr/share/nginx/html/assets
RUN mkdir /usr/share/nginx/html/assets
RUN mv /usr/share/nginx/html/i18n /usr/share/nginx/html/assets

# Start NgInx service
CMD ["nginx", "-g", "daemon off;"]