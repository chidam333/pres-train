morning task :

```sql
select * from actor;

select title, length, rental_rate 
from film 
order by length desc;

select r.customer_id, count(*) as total_rentals 
from rental r 
join customer c on r.customer_id = c.customer_id 
group by r.customer_id 
order by total_rentals desc 
limit 5;

select f.film_id, f.title 
from film f 
left join inventory i on f.film_id = i.film_id 
left join rental rt on i.inventory_id = rt.inventory_id 
where rt.rental_id is null;

select f.title, concat(a.first_name, ' ', a.last_name) as full_name from
film f join film_actor fa on f.film_id = fa.film_id 
join actor a on fa.actor_id = a.actor_id 
where f.title like 'Academy Dinosaur';

select count(*) as total_rentals, sum(p.amount) as total_amount, c.first_name 
from customer c 
join rental r on c.customer_id = r.customer_id 
join payment p on c.customer_id = p.customer_id 
group by c.customer_id;

with top_rentals as (
	select f.title, count(r.rental_id) as total_count 
	from film f 
	join inventory i on f.film_id = i.film_id 
	join rental r on i.inventory_id = r.inventory_id 
	group by f.title
)

select title, total_count 
from top_rentals 
order by total_count 
limit 3;

with customer_rentals as (
	select customer_id, count(*) as total_rentals 
	from rental 
	group by customer_id
)
select customer_id, total_rentals 
from customer_rentals 
where total_rentals > (
	select avg(total_rentals) 
	from customer_rentals
);

select * from rental;

create or replace function get_rentals(c_id int)
returns int as $$
declare
	total_rentals int;
begin 
	select count(*) into total_rentals 
	from rental 
	where customer_id = get_rentals.c_id;
	return total_rentals;
end
$$ language plpgsql;

drop function get_rentals(integer);

select get_rentals(222);

create procedure updaterate(id int,newrate int)
language plpgsql
as $$
begin 
	update film set rental_rate=updaterate.newrate where film_id=updaterate.id;
end
$$

select film_id,rental_rate from film;
call updaterate(1,2)
select film_id,rental_rate from film where film_id=1;

```

evening task:
```sql
-- Trigger task

create table newcustomerlog(
    insert_time time default now(),
    name varchar
);


create or replace function logcustomer()
returns trigger as $$
begin
	insert into newcustomerlog values (now(),NEW.first_name);
	return New;
end;
$$ language plpgsql;

create or replace trigger customerinsert 
after insert on customer
for each row
execute function logcustomer();

insert into customer (store_id, first_name, last_name, email, address_id, activebool, create_date, last_update, active)
values (1, 'Kane', 'Harry', 'Harrykane@tot.com', 100, TRUE, '2025-05-09', '2025-05-09', 1);

select * from newcustomerlog;

create or replace function prevent_0_pay()
returns trigger as $$
begin
	if New.amount=0.00 then 
		raise exception 'Pay more than 0 bro?';
	end if;
	return New;
end;
$$ language plpgsql;

create or replace trigger pay_trigger 
before insert on payment
for each row
execute function prevent_0_pay();

insert into payment (payment_id, customer_id, staff_id, rental_id, amount, payment_date)
VALUES (1, 101, 1, 210, 0.00, '2025-05-09');


create or replace function lu_func()
returns trigger as $$
begin
	New.last_update=NOW();
	RAISE INFO 'last update set successfully';
	return New;
end;
$$ language plpgsql;

create or replace trigger lu_trigger
after update on film
for each row
execute function lu_func();

select * from film;
update film set title='Holes1' where film_id=420;

create or replace function inv_log()
returns trigger as $$
begin
	if TG_OP='INSERT' then
		Raise INFO 'new data in inventory with id %',New.inventory_id;
		return New;
	end if;
	if TG_OP='DELETE' then 
		Raise INFO 'inventory with id % has been deleted',Old.inventory_id;
		return New;
	end if;
	return null;
end;
$$ language plpgsql;

create or replace trigger inv_trigger
after insert on inventory
for each row 
execute function inv_log();

create or replace trigger inv_delete
after delete on inventory
for each row 
execute function inv_log();

select * from information_schema.columns where table_name='inventory';
select max(inventory_id) from inventory;
insert into inventory values (9999,999,999);
delete from inventory where inventory_id=9999;


-- Transaction task

DO $$
DECLARE
    v_new_customer_id INT;
BEGIN
    INSERT INTO public.customer (store_id, address_id, first_name, last_name, email, create_date)
    VALUES (1, 1, 'Shri', 'Ram', 'ram@gmail.com', NOW())
    RETURNING customer_id INTO v_new_customer_id;

    INSERT INTO public.rental (rental_date, inventory_id, customer_id, staff_id, return_date)
    VALUES (NOW(), 1, v_new_customer_id, 1, NULL);
END;
$$;

DO $$
BEGIN
    UPDATE public.film 
    SET rental_rate = 5.99
    WHERE film_id = 1;

    INSERT INTO public.inventory (film_id, store_id)
    VALUES (1, 1);

EXCEPTION
    WHEN OTHERS THEN
        RAISE NOTICE 'Error occurred. Transaction is being rolled back.';
        RAISE;
END;
$$;

DO $$
DECLARE
    inventory_item_id INT := 1;
    source_store INT := 1;
    target_store INT := 2;
BEGIN
    UPDATE public.inventory
    SET store_id = target_store
    WHERE inventory_id = inventory_item_id AND store_id = source_store;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'Inventory ID % not found in store %', inventory_item_id, source_store;
    END IF;

    RAISE NOTICE 'Inventory % transferred from store % to store %', inventory_item_id, source_store, target_store;
END;
$$;

BEGIN;
    UPDATE public.payment
    SET amount = amount + 5
    WHERE payment_id = 1;
SAVEPOINT sp_payment;
    UPDATE public.payment
    SET amount = amount + 10
    WHERE payment_id = 2;
ROLLBACK TO SAVEPOINT sp_payment;
COMMIT;

DO $$
DECLARE
    cust_id INT := 100;
BEGIN
    DELETE FROM public.payment
    WHERE customer_id = cust_id;
    DELETE FROM public.rental
    WHERE customer_id = cust_id;
    DELETE FROM public.customer
    WHERE customer_id = cust_id;
END;
$$;
```