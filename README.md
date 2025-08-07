# JiaSyuanLibrary.Net

## AutoMappingHelper (DI 版本)

本版本整合 AutoMappingHelper 與 AutoMapper 的 DI 擴充，透過模組化 Profile 註冊機制，簡化 AutoMapper 初始化流程與模組管理。

---

### 🔌 DI 整合說明

可透過 `AddAutoMapperWithProfiles(...)` 擴充方法，註冊映射模組與 Profile。

#### ✅ 使用範例
```csharp
// 手動註冊模組
builder.Services.AddAutoMapperWithProfiles(reg =>
{
    AutoMapperModules.RegisterModules(reg);
});

// 自動掃描並註冊模組
builder.Services.AddAutoMapperWithProfiles(reg =>
{
    AutoMapperModules.RegisterModulesAuto(reg);
});


```

# JiaSyuanLibrary.NetFramework

TargetFrameworkVersion:4.8.1

## AutoMappingHelper

AutoMappingHelper 是一個以 AutoMapper 為核心的封裝工具，支援物件映射、集合轉換、Profile 註冊與快取等功能。簡化日常開發中的 DTO / ViewModel 轉換流程，並提升映射可讀性與擴充性。

---

### ✨ 特點特色

- ✅ 支援單一物件、集合、合併來源映射
- ✅ 結合 MappingProfileRegistry 進行 Profile 註冊管理
- ✅ 透過 MapperFactory 實現 IMapper 快取與延遲建立
- ✅ 相容 AutoMapper 最新版本（含 ILoggerFactory）

---

### 🔧 安裝套件

請確保已安裝 AutoMapper 相關套件：

```bash
dotnet add package AutoMapper
dotnet add package Microsoft.Extensions.Logging.Abstractions
