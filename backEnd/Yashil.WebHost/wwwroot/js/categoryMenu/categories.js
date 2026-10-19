async function fetchMenuList() {
    const categoriesResponse = await fetch('/api/PublicApi/getMenu');
    const categoriesWithRoot = await categoriesResponse.json();
    if (categoriesWithRoot && categoriesWithRoot.length > 0) {
        const categories = categoriesWithRoot[0].subCategories;
        return categories;
    }
    return [];
}