

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





