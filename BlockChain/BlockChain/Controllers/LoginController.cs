﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockChain.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LoginController : ControllerBase
  {
    // GET: api/Login
    [HttpGet]
    public int Get()
    {
      return 200;
    }

    // GET: api/Login/5/Jan
    [HttpGet("{id}/{Name}", Name = "Get")]
    public string Get(int id, string Name)
    {
      return "value" + id + "name: " + Name;
    }

    // POST: api/Login
    [HttpPost]
    public string Post([FromBody] LoginCredentials Credentials)
    {
      return Credentials.getKey();
    }

    // PUT: api/Login/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {

    }

    // PUT: api/Login
    [HttpPut]
    public string Put([FromBody] LoginCredentials Credentials)
    {
      return Credentials.getKey();
    }

    // PUT: api/Login/
    [HttpPut("{node}")]
    public void Put(string IP, string RSA, DateTime time)
    {

    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}

public class LoginCredentials
{
  string key;
  public LoginCredentials(string key)
  {
    this.key = key;
  }

  public string getKey()
  {
    return this.key;
  }
}
