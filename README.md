# WebApiWithJWT
This project demonstrates how to implement JWT in a asp.net Web api project

## Program.cs
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero // No clock skew tolerance
            // There is a concept called Clock skew, this allows the comparingson between server and the token-
            // experation time to be a little off. by default there is a built in clockskew tolerance, in the validation process
            // However by setting the TimeSpan.Zero, it removes the allowance for tolerance in the server and token time difference

            /* IMPORTANT USE CASE OF SETTING THE CLOCKSKEW to TimeSpan.Zero
               For applications where users might experience slight time discrepancies (e.g., different time zones),
               setting ClockSkew to TimeSpan.Zero could lead to a less forgiving user experience,
               as tokens might be rejected due to minor time differences
            */

        };
    });
```

## Creating the token
```csharp
        private string GenerateToken(UserDTO userDTO)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDTO.Username),
                new Claim(ClaimTypes.NameIdentifier, userDTO.UID)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
```

## appsettings
### Remember to include this in appsettings
```json
  /* Setting the values of the JWT options*/
  "Jwt": {
    "Issuer": "org.meyer100",
    "Audience": "org.meyer100",
    /* Make sure to make a long an unique key
       Also it is not recommended to keep the key inside the appsettings
    */
    "Key": "9KXaZPbsY9!6C@CEbz%&FC3pknb7RMYKgRRAN%A^nFiWCfQauAyCu9pPxLsFG%RckNjntHVQezV!5$L9ZDNE"
  },
```

## Which packages do i need?
### For .6 projects
Microsoft.AspNetCore.Authentication.JwtBearer version 6.0.23
### For .7 or newer versions
Microsoft.AspNetCore.Authentication.JwtBearer Latest
