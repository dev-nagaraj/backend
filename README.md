# Untitled

**Github URL** : [https://github.com/dev-nagaraj/backend#](https://github.com/dev-nagaraj/backend#)

### Prerequisites:

- Dot Net core 10
- Postgres with PgAdmin 16 +
- Visual Studio 2026

### Database Design:

<img width="693" height="682" alt="image" src="https://github.com/user-attachments/assets/724f96da-cb3a-4a8a-a28b-0ae78614f6a2" />


```sql
BEGIN;

CREATE TABLE IF NOT EXISTS public.customer
(
    customer_id text COLLATE pg_catalog."default" NOT NULL,
    region text COLLATE pg_catalog."default" NOT NULL,
    customer_name text COLLATE pg_catalog."default" NOT NULL,
    customer_email text COLLATE pg_catalog."default" NOT NULL,
    customer_address text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT customer_pkey PRIMARY KEY (customer_id)
);

CREATE TABLE IF NOT EXISTS public.details_raw
(
    order_id integer,
    product_id text COLLATE pg_catalog."default",
    customer_id text COLLATE pg_catalog."default",
    date_of_sale timestamp without time zone,
    quantity_sold integer,
    unit_price numeric,
    discount numeric,
    shipping_cost numeric,
    payment_method text COLLATE pg_catalog."default",
    product_name text COLLATE pg_catalog."default",
    category text COLLATE pg_catalog."default",
    region text COLLATE pg_catalog."default",
    customer_name text COLLATE pg_catalog."default",
    customer_email text COLLATE pg_catalog."default",
    customer_address text COLLATE pg_catalog."default"
);

CREATE TABLE IF NOT EXISTS public.orders
(
    order_id integer NOT NULL,
    product_id text COLLATE pg_catalog."default" NOT NULL,
    customer_id text COLLATE pg_catalog."default" NOT NULL,
    date_of_sale timestamp without time zone DEFAULT now(),
    quantity_sold integer DEFAULT 1,
    unit_price numeric,
    discount numeric,
    shipping_cost numeric,
    payment_method text COLLATE pg_catalog."default",
    CONSTRAINT orders_pkey PRIMARY KEY (order_id)
);

CREATE TABLE IF NOT EXISTS public.product
(
    product_id text COLLATE pg_catalog."default" NOT NULL,
    product_name text COLLATE pg_catalog."default" NOT NULL,
    category text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT product_pkey PRIMARY KEY (product_id)
);

ALTER TABLE IF EXISTS public.orders
    ADD CONSTRAINT fk_customer_id FOREIGN KEY (customer_id)
    REFERENCES public.customer (customer_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;

ALTER TABLE IF EXISTS public.orders
    ADD CONSTRAINT "fk_product_Id" FOREIGN KEY (product_id)
    REFERENCES public.product (product_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;

END;

CREATE or replace  PROCEDURE adddata()
LANGUAGE plpgsql
AS $$
BEGIN
   COPY public.details_raw FROM 'C:\Users\codew\OneDrive\Desktop\details.csv' DELIMITER ';' CSV HEADER;
END;
$$;

CREATE or replace  PROCEDURE refresh_sales_data()
LANGUAGE plpgsql
AS $$
BEGIN
   Insert  into product 
   select distinct product_id, category , product_name
   from public.details_raw
   where last_updated_at < NOW()
   ON Conflict DO Nothing;

   Insert  into customer 
   select distinct customer_id, region, customer_name, customer_email, customer_address
   from public.details_raw
   where last_updated_at < NOW()
   ON Conflict DO Nothing;

   Insert  into orders (order_id,customer_id, product_id,date_of_sale, quantity_sold, shipping_cost,unit_price,discount,payment_method)
   select distinct order_id,customer_id, product_id,date_of_sale, quantity_sold, shipping_cost,unit_price,discount,payment_method
   from public.details_raw
   where last_updated_at < NOW();

   UPDATE public.customer set last_updated_at = NOW();
   UPDATE public.product set last_updated_at = NOW();
   UPDATE public.orders set last_updated_at = NOW();
   
END;
$$;

CREATE OR REPLACE FUNCTION GetTotalRevenue(revenueType text, from1 TIMESTAMP, to1 TIMESTAMP) 
RETURNS decimal AS $$
    BEGIN
           select  
            SUM ((o.unit_price - o.discount) * o.quantity_sold + o.shipping_cost) as revenue 
           from orders o left join public.product p on p.product_id = o.product_id
           left join public.customer c on c.customer_id = o.customer_id
           where date_of_sale between from1 AND to1
            if revenueType == 2
                Group By (p.product_name);
            else if revenueType = 3
                Group By (p.category)
            else if revenueType = 4
                Group By (c.region)
            END IF; 
    END;
$$ LANGUAGE plpgsql;

```

### Running Steps:

1. Clone the repo from github also verify the Prerequisites all installed.
2. Run the above script in the pgadmin
3. Set the startup project as API in the Visual studio and start the application
4. There are two endpoints
    1. **api/GetRevenue/{revenuetype}?from={datetime}&to={datetime}** which accept the three param as input **RevenueType, From** and **To.**
        
        It  will return the revenue for following details
        
       <img width="271" height="192" alt="image" src="https://github.com/user-attachments/assets/4aa4d7de-27ff-417f-b62b-3d8565d8c9da" />

        
    2. **api/refresh** this will sync the latest data from raw table to respective table
5. This solution completely based on the ELT and its completely done at posgres not in application level.

### Sample Request Response

1. **api/GetRevenue/{revenuetype}?from={datetime}&to={datetime}

Request :**[https://localhost:50123](https://www.notion.so/)/api/GetRevenue/1?from=10-10-2025&to=15-10-2025

**Response:** 
    
    ```json
    {
    RevenueDetail:{
        Amount :8082
        }
    }
    ```
    
2. **api/refresh

Request :**[https://localhost:50123](https://www.notion.so/)/api/GetRevenue/1?from=10-10-2025&to=15-10-2025

**Response: Refreshed successfully**
