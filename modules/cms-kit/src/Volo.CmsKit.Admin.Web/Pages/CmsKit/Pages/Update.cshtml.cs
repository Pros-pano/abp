﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages
{
    public class UpdateModel : CmsKitAdminPageModel
    {
        [BindProperty(SupportsGet = true)]
        [HiddenInput]
        public Guid Id { get; set; }

        [BindProperty]
        public UpdatePageViewModel ViewModel { get; set; }

        protected readonly IPageAdminAppService pageAdminAppService;

        public UpdateModel(IPageAdminAppService pageAdminAppService)
        {
            this.pageAdminAppService = pageAdminAppService;
        }

        public async Task OnGetAsync()
        {
            var dto = await pageAdminAppService.GetAsync(Id);

            ViewModel = ObjectMapper.Map<PageDto, UpdatePageViewModel>(dto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var updateInput = ObjectMapper.Map<UpdatePageViewModel, UpdatePageInputDto>(ViewModel);

            await pageAdminAppService.UpdateAsync(Id, updateInput);

            return NoContent();
        }

        [AutoMap(typeof(PageDto))]
        [AutoMap(typeof(UpdatePageInputDto), ReverseMap = true)]
        public class UpdatePageViewModel
        {
            [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxTitleLength))]
            [Required]
            public string Title { get; set; }

            [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxSlugLength))]
            [Required]
            [DynamicFormIgnore]
            public string Slug { get; set; }
        }
    }
}