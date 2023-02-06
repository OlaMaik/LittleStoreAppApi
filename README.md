**LittleStoreAPI**
### Introduction
This project was created to help users with check calculation. 
### API Endpoints
POST /store/start To calculate check based on store assortment and user`s order. Method requires two text files. Method returns text file as a successful response.
### Files requirements
File with assortment must have "PRODUCT|PRICE" header. Next lines must be filled with list of products and prices (1 product per line). For example (Tomato|1,3). File with orders must have "PRODUCT|AMOUNT" header. Next lines must be filled with list of products and amounts(1 product per line). For example (Tomato|1).
Only products from list are acceptable: Tomato, Carrot, Banana, Lemon, Watermelon. Other types of products will be ignored. Notification about skipped products will be added to check.
### Offers
As long as was decided not to use db, separate http method for offers creation could not be created. This is why offers are created as a collection during execution of Start() method. Available offers: Buy 2 banana, get lemon as a present, Buy 3 watermelons, get 50% discount for carrots
