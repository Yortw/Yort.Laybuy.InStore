# Introduction


This library is a thin/light weight OOP wrapper around the [Laybuy API](https://integrations.laybuy.com/reference). It provides a clean OOP style interface and saves you having to do generate your own models, make HTTP calls, serialise/deserialise requests and responses and so on. It doesn't add any retry logic, persistent stores or reliability handling. It is still up to the application developer to provide a [robust implementation](articles/productionrequirements.html).

This library only implements the [Laybuy in-store/POS API](https://integrations.laybuy.com/reference#point-of-sale-integration-flow) and does not implement e-commerce flows. 

