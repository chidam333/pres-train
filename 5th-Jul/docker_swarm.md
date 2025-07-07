```
docker swarm init
```

```
docker service create --name nginx-web --replicas 3 -p 8080:80 nginx
```


```
docker service ls
```
![alt text](image.png)



```
docker service ps nginx-web
```
![alt text](image2.png)

```
curl localhost:8080
```
![alt text](image3.png)

```
docker service scale nginx-web=5
```