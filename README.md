## Knock.API - Restaurant rating REST API

Bu projeyi ASP.NET Core 3.1 kullanarak yapıyorum. Lokantalari degerlendirmek icin olusturdum. Lokantalarin verilerine CRUD islemleri yapilabiliyor ve bu lokantalara yorumlar da eklenebiliyor.

#### Teknolojiler

- Editor
  - VSCode
- Sdk
  - ASP.Net 3.1 SDk
- Backend Programlama
  - C#
  - ASP.Net Core 3.1
- Veritabani
  - Sqlite - basit bir uygulama oldugu icin Sqlite kullanilmasi daha mantikli oldugunu dusundum (Sonra SqlServer'e gecmeye dusunuyorum)

#### Routelar

Bu projede iki temel route var, lokantalar ve yorumlar icin. Once lokantalarin verileri girilecek sonra o lokantalarin yorumlari eklenecektir.

##### Lokantalar

Lokantalar icin bir kac tane sutun vardir.

```json
{
  "name": "Pizza Palace",
  "address": "125 Palace Road",
  "website": "https://pizzapalace.com", // nullable
  "offerstakeout": false
}
```

Temel lokanta routelar bunlardir.

```yaml

  - GET
    - "/api/restaurants" # tum lokantalarin verileri almak icin
    - "/api/restaurants/{restaurantId}" # bir lokantanin verileri almak icin (Guid kullanarak)
  - POST
    - "/api/restaurants" # veriler girmek icin
  - PUT
    - "/api/restaurants/{restaurantId}" # guncellemek icin
  - DELETE
    - "/api/restaurants/{restaurantId}" # silmek icin

```

Bir den fazla lokanta verileri girmek icin bir route olusturdum

```yaml
- GET
  - "/api/restaurantcollections/({ids})" # virgul ile ayirarak birden fazla lokanta id girilebilir
- POST
  - "/api/restaurantcollections" # birden fazla lokanta verileri girilebilir
```

```json
// veriler boyle girilir
[
  {
    "name": "Pizza Palace",
    "address": "125 Palace Road",
    "website": "https://pizzapalace.com", // nullable
    "offerstakeout": false
  },
  {
    "name": "Lahmacun Palace",
    "address": "185 Palace Road",
    "website": "https://lahmacunpalace.com", // nullable
    "offerstakeout": true
  }
]
```

Id'ler icin Guid kullaniyorum, repository icinde girilen Guid kendimi tanimliyorum.

##### Yorumlar

Yorumlarin veriler boyle girilir

```json
{
  "content": "This place is super awesome", // yorum
  "rating": 2 // degerlendirme - 0 ve 5 arasinda
}
```

```yaml

  - GET
    - "/api/restaurants/{restaurantId}/reviews" # yorumlar almak
    - "/api/restaurants/{restaurantId}/reviews/{reviewId}" # bir yorum
  - POST
    - "/api/restaurants/{restaurantId}/reviews" # veriler girmek icin
  - PUT
    - "/api/restaurants/{restaurantId}/reviews/{reviewId}" # guncellemek icin
  - DELETE
    - "/api/restaurants/{restaurantId}/reviews/{reviewId}" # silmek icin

```

#### Klasor Yapimi

Bu projede klasor yapimi degisiktir. Tum modeller entity klasordedir ve model klasorde Dto'lar vardir.

##### Services

Services klasorde iki dosya var, `IKnockRepository` ve `KnockRepository`. `IKnockRepository` bir interface siniftir, bunun icinde tum metotlar olusturdum. `KnockRepository` ise `IKnockRepository` icinde olusturdugum metotlar tanimladim. Bu repolar kullanarak veritabanindan veriyi aliyorum ve controller icinde sadece bu repolarin metotlari kullanarak islemi yapabiliyorum.

- Services
  - IKnockRepository.cs
  - KnockRepository.cs

##### Entities

Modellerin dosyalari var. Bu siniflari kullanarak veritabaninda tablolar olusturdum.

- Entities
  - Restaurant.cs
  - Review.cs

##### Profiles

Automapper icin kullaniyorum. Dto ve Model arasinda mapping icin kullaniliyor.

##### Helpers

ArrayModelBinder sinif ile girilen veriler model ile iliskiyi kuruyor ve bu verileri kullanarak veritabaninda olan verileri alinabiliyor.

Mesela, `/api/restaurantcollections/({ids})` boyle bir uri girersek, bu idler olan lokantalarin verileri alabiliyoruz

#### Eklenecekler

Bu projeyi hala yapiliyor ve bir kac gelistirmeyi yapmaya dusunuyorum zamanla.

- [ ] SqlServer'i veritabani olarak kullanacagim
- [ ] Search, Filter ve Pagination
- [ ] Authentication ve Authorization - Identity ya da JWT kullanarak
- [ ] XML Request ve Response destek `// oldugunu dusunuyorum`
- [ ] Docker kullanarak mikroservise ayirmak - Restaurants ve Reviews iki servise ayirarak
- [ ] Hosting - Azure Cloud
