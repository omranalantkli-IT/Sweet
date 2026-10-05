# نظام إدارة معمل الحلويات

نظام محلي لإدارة العمال والإنتاج اليومي وحساب المستحقات الشهرية.

## البنية

تفاصيل تقسيم الطبقات والميزات وقواعد تنظيم الملفات موضحة في [ARCHITECTURE.md](ARCHITECTURE.md).

- `backend`: ASP.NET Core Web API، JWT، صلاحيات Admin/Worker، وSQL Server للسيرفر، مع دعم SQLite للتطوير المحلي.
- `frontend`: React + Vite، واجهة عربية RTL ومسارات محمية حسب الصلاحية.

## التشغيل

انسخ ملف الإعدادات النموذجي أولًا، ثم غيّر مفتاح JWT وكلمة مرور المدير:

```powershell
Copy-Item backend\appsettings.example.json backend\appsettings.json
```

شغّل الباك إند:

## إعداد SQL Server على السيرفر

اضبط `DatabaseProvider` إلى `SqlServer` في إعدادات السيرفر، وعيّن `ConnectionStrings:DefaultConnection` باسم SQL Server الفعلي. للإنتاج استخدم `ASPNETCORE_ENVIRONMENT=Production`؛ إعدادات Development تستخدم `(localdb)\MSSQLLocalDB` مع Windows Authentication وقاعدة `sweet`.

LocalDB للتطوير المحلي فقط ويعمل ضمن حساب Windows الحالي، وليس بديلًا عن خدمة SQL Server على سيرفر الإنتاج. `TrustServerCertificate=True` موجود في إعداد التطوير المحلي فقط. يمكن الرجوع إلى SQLite بتعيين `DatabaseProvider=Sqlite` وسلسلة الاتصال `Data Source=sweet-factory.db`.

مثال Windows Authentication:

```text
Server=YOUR_SQL_SERVER;Database=sweet;Integrated Security=True;Encrypt=True;TrustServerCertificate=False
```

مثال SQL Authentication (احفظ كلمة المرور في متغير البيئة `ConnectionStrings__DefaultConnection` وليس في Git):

```text
Server=YOUR_SQL_SERVER;Database=sweet;User Id=YOUR_USER;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=False
```

يجب منح حساب التطبيق الصلاحيات اللازمة لإنشاء قاعدة جديدة وجداولها عند التشغيل الأول. استخدم قاعدة `sweet` جديدة فارغة؛ لا يغيّر التطبيق مخطط قاعدة موجودة تلقائيًا. استخدم شهادة TLS موثوقة، ولا تعتمد `TrustServerCertificate=True` في الإنتاج. بيانات SQLite القديمة لا تُنقل تلقائيًا، وملفاتها محفوظة محليًا وغير مرفوعة.

## تشغيل التطبيق

```powershell
cd D:\Sweet\backend
dotnet run
```

ثم شغّل الواجهة في نافذة ثانية:

```powershell
cd D:\Sweet\frontend
npm run dev
```

افتح `http://localhost:5173`.

## الحسابات التجريبية

- المدير: `admin` / `Admin@123`
- العامل التجريبي: `ahmad` / `Worker@123`

غيّر كلمة مرور المدير ومفتاح JWT قبل أي نشر فعلي.

## دورة العمل

1. المدير ينشئ حساب العامل.
2. المدير يضيف أنواع العمل وسعر الوحدة.
3. العامل يدخل نوع العمل والكمية والتاريخ.
4. النظام يحتفظ بسعر الوحدة وقت التسجيل.
5. المدير يراجع سجل الإنتاج ومستحقات كل عامل شهريًا.
