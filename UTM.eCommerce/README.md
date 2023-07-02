# Code Smells:

Duplicated Code: The code checks for an existing product with the same name twice, both in the CreateProduct and CheckIfProductExists methods.
Long Method: The CreateProduct method handles multiple responsibilities, including product creation, customer retrieval, and order creation.
Primitive Obsession: The GetOrCreateCustomer method uses primitive values (hard-coded names) to find or create a customer.

# Design Smells:

God Object: The CreateProduct method handles product creation, customer retrieval, and order creation, violating the Single Responsibility Principle.
Shotgun Surgery: The stock count reduction and order creation logic is scattered across multiple methods, making it difficult to modify in case of changes.
Lack of Abstraction: The GetOrCreateCustomer method directly accesses the repository and does not use interfaces or abstractions to shield higher-level modules from low-level implementation details.