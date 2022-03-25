# creating microservices.

## Some useful mongo commands

```Shell
    use CatalogDB #to created db on mongo
    db.createCollection('products') #to create products collection

    show collection #to views the collections
```

```shell
    #inserting data to the collection
    db.products.insert({json strings})  # will insert 1 data to the collections
    db.products.insertMany([list of json string]) # will insert list of data to the collections.
```

```shell
    # now for selecting the data in collections.
    db.products.find({}).pretty() # equivalent to select * from products
```

```shell
     db.Products.remove({})   
```
