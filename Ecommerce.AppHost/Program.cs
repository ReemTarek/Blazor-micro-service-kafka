var builder = DistributedApplication.CreateBuilder(args);
var productApi = builder.AddProject<Projects.Ecommerce_ProductService>("apiservice-product"); 
var orderApi = builder.AddProject<Projects.ECommerce_OrderService>("apiservice-order");
var paymentApi= builder.AddProject<Projects.ECommerce_PaymentService>("ecommerce-paymentservice");
builder.AddProject<Projects.Ecommerce_Web>("webfrontend").WithReference(productApi).WithReference(orderApi).WithReference(paymentApi);
builder.Build().Run();
