# TeLoArreglo Rest-API reference

## Index

- [1 Authorization (Log in)](#1-authorization) 
- [2 Logout](#2-logout)
- [3 Create new user or User registration](#3-create-new-user-or-user-registration)
- [4 Get actions of user](#4-get-actions-of-user)


## 1 Authorization (Log in)

All API requests require the use of a generated API key using a header called `x-auth-token`. You can generate your new api key using the login endpoint.

### 1.1 Request

```http
POST /api/login
```

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| `username` | `string` | **Required**. Username of the user that is going to log in |
| `password` | `string` | **Required**. Password of the user that is going to log in.

##### Example

```
{
  "username" : "admin",
  "password" : "123456"
}
```

### 1.2 Response

##### Example
```
{
    "token": "499eedc1-8e42-44b1-9e69-83139abd12d6"
}
```

##### Status Codes
TeLoArreglo returns the following status codes in this endpoint:

| Status Code | Type | Description |
|-------------| ----------- | ----------- | 
| 200 | `OK` | Ok |
| 404 | `NOT FOUND` | Incorrect username or password |
| 500 | `INTERNAL SERVER ERROR`| Unexpected error (probably DB related) |

###### Example error response (404)

```
Incorrect username or password.
```

## 2 Logout

### 1.1 Request

It doesn't need any body content, you just need to have the header `x-auth-token: API_KEY` that the login returned.

```http
POST /api/logout
```

### 1.2 Response

##### Example

```
{
    "token": "499eedc1-8e42-44b1-9e69-83139abd12d6"
}
```

##### Status Codes
TeLoArreglo returns the following status codes in this endpoint:

| Status Code | Type | Description |
|-------------| ----------- | ----------- | 
| 200 | `OK` | Ok |
| 400 | `BAD REQUEST` | Api key not found in header |
| 401 | `UNAUTHORIZED` | Not logged in |
| 500 | `INTERNAL SERVER ERROR`| Unexpected error (probably DB related) |

## 3 Create new user or User registration

This endpoint was created for both purposes. The only difference is that you need to have the header `x-auth-token: API_KEY` that the login returned and your user an `Admin` role if you want to create an user with a complex role (`Admin` or `Crew`). If this is not the case, the API will automatically create the new user with the `User` role instead of the one specified in the request.

### 3.1 Request

```http
POST /api/users
```

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| `username` | `string` | **Required**. Username of the user that is going to be created |
| `password` | `string` | **Required**. Password of the user that is going to be created |
| `role` | `string` | **Required if `role != User`**. Role of user that is going to be created (`Admin`, `Crew` or `User`)|

##### Example

```
{
    "username": "admin",
    "password": "123456"
    "role": "Admin"
}
```

### 3.2 Response

##### Example

```
{
    "username": "admin",
    "role": "Admin",
    "id": 1
}
```

##### Status Codes
TeLoArreglo returns the following status codes in this endpoint:

| Status Code | Type | Description |
|-------------| ----------- | ----------- | 
| 200 | `OK` | Ok |
| 400 | `BAD REQUEST` | Api key not found in header |
| 401 | `UNAUTHORIZED` | Not logged in |
| 403 | `FORBIDDEN` | Insufficient privileges |
| 500 | `INTERNAL SERVER ERROR`| Unexpected error (probably DB related) |

## 4 Get Actions of user

You need to have the header `x-auth-token: API_KEY` that the login returned.

### 4.1 Request

```http
GET /api/actions
```

### 4.2 Response

##### Example
```
[0,1,2]
```

##### Status Codes
TeLoArreglo returns the following status codes in this endpoint:

| Status Code | Type | Description |
|-------------| ----------- | ----------- | 
| 200 | `OK` | Ok |
| 400 | `BAD REQUEST` | Api key not found in header |
| 401 | `UNAUTHORIZED` | Not logged in |
| 500 | `INTERNAL SERVER ERROR`| Unexpected error (probably DB related) |
